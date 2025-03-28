import cv2
import mediapipe as mp
import socket
import time

# Setup MediaPipe
mp_hands = mp.solutions.hands
hands = mp_hands.Hands(max_num_hands=1, min_detection_confidence=0.7)
mp_draw = mp.solutions.drawing_utils

cap = cv2.VideoCapture(0)

# Configs
cooldowns = {
    "MOVE_LEFT": 0.4,
    "MOVE_RIGHT": 0.4,
    "SOFT_DROP": 0.6,
    "ROTATE": 1.0,
    "HARD_DROP": 2.0,  # longer cooldown for safety
}
required_stable_frames = 8  # must detect same gesture for N frames

# State
last_command = None
last_sent_time = 0
stable_command = None
stable_frames = 0
hold_start_time = None

# Prediction freeze after command
prediction_lock_until = 0


def get_finger_state(landmarks):
    finger_states = []
    tips_ids = [4, 8, 12, 16, 20]
    for i in tips_ids:
        tip = landmarks[i]
        pip = landmarks[i - 2]
        finger_states.append(tip.y < pip.y)
    return finger_states


while True:
    ret, frame = cap.read()
    image = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
    result = hands.process(image)

    detected_command = None
    current_time = time.time()

    # Only predict if not locked
    if current_time >= prediction_lock_until and result.multi_hand_landmarks:
        for hand_landmarks in result.multi_hand_landmarks:
            mp_draw.draw_landmarks(frame, hand_landmarks, mp_hands.HAND_CONNECTIONS)
            landmarks = hand_landmarks.landmark

            thumb_tip = landmarks[4]
            thumb_base = landmarks[2]
            index_tip = landmarks[8]
            wrist = landmarks[0]

            finger_states = get_finger_state(landmarks)

            # 👍 Thumbs-up gesture pointing down (only thumb down, others folded)
            if thumb_tip.y > thumb_base.y and not any(finger_states[1:]):
                detected_command = "HARD_DROP"
            elif all(finger_states):  # ✋ All fingers up
                detected_command = "ROTATE"
            elif wrist.y > 0.8:
                detected_command = "MOVE_DOWN"
            elif thumb_tip.x < index_tip.x - 0.05:
                detected_command = "MOVE_LEFT"
            elif thumb_tip.x > index_tip.x + 0.05:
                detected_command = "MOVE_RIGHT"


    # Stability tracking (based on consistent frames)
    if detected_command == stable_command:
        stable_frames += 1
    else:
        stable_command = detected_command
        stable_frames = 1
        hold_start_time = current_time

    held_duration = current_time - (hold_start_time or current_time)
    cooldown = cooldowns.get(stable_command, 1.0)

    # Send if gesture is stable and cooldown passed
    if (
        stable_command
        and stable_frames >= required_stable_frames
        and (
            stable_command != last_command or (current_time - last_sent_time) > cooldown
        )
    ):
        print(f"✔ Stable gesture — Sending: {stable_command}")
        try:
            with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as sock:
                sock.connect(("localhost", 5050))
                sock.sendall(stable_command.encode())
        except Exception as e:
            print("⚠️ Connection error:", e)

        last_command = stable_command
        last_sent_time = current_time
        stable_frames = 0
        prediction_lock_until = current_time + 0.5  # slight delay before next detection

    # Display
    cv2.putText(
        frame,
        f"Gesture: {stable_command or 'None'} ({stable_frames} frames / {held_duration:.1f}s)",
        (10, 30),
        cv2.FONT_HERSHEY_SIMPLEX,
        0.7,
        (0, 255, 0),
        2,
    )

    # Cooldown bar
    if last_command:
        elapsed = current_time - last_sent_time
        cd = cooldowns.get(last_command, 1.0)
        progress = min(elapsed / cd, 1.0)
        cv2.rectangle(
            frame, (10, 50), (10 + int(300 * progress), 70), (0, 255, 255), -1
        )
        cv2.putText(
            frame,
            f"Cooldown: {last_command}",
            (10, 45),
            cv2.FONT_HERSHEY_SIMPLEX,
            0.5,
            (0, 200, 200),
            1,
        )

    cv2.imshow("Hand Gesture", frame)
    if cv2.waitKey(1) & 0xFF == ord("q"):
        break

cap.release()
cv2.destroyAllWindows()
