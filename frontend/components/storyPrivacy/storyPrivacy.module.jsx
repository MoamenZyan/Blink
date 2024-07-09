"use client"
import styles from "./storyPrivacy.module.css";
import { useRef } from "react";
export default function StoryPrivacy({setPrivacy, setPrivacyPopup, handleIcon, text, setOverlay}) {
    const select = useRef(null);

    const handleOnSave = () => {
        setPrivacy(select.current.value);
        setPrivacyPopup(false);
        handleIcon(select.current.value);
        if (setOverlay) {
            setOverlay(false);
        }
    }

    return (
        <>
            <div className={styles.box}>
                <h3>Choose {text} privacy: </h3>
                <select id="privacy" ref={select}>
                    <option value="null">--Select {text} privacy--</option>
                    <option value="public">public</option>
                    <option value="friends">friends</option>
                    <option value="private">private</option>
                </select>
                <div className={styles.buttons}>
                    <button onClick={() => {
                        setPrivacyPopup(false);
                        handleIcon();
                        if (setOverlay) {
                            setOverlay(false);
                        }
                        }}>CANCEL</button>
                    <button onClick={handleOnSave}>SAVE</button>
                </div>
            </div>
        </>
    )
}