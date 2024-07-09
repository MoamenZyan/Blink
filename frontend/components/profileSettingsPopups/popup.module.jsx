"use client"
import styles from "./popup.module.css";
import { useRef } from "react";

function handleType (type, first, last) {
    switch (type) {
        case 'about':
            return (<div className={styles.textarea_div}>
                <textarea ref={first} placeholder="edit about..."/>
            </div>)
        case 'name':
            return (
            <div className={styles.name_div}>
                <input ref={first} type="text" placeholder="enter first name..."/>
                <input ref={last} type="text" placeholder="enter last name..."/>
            </div>
            )
        case 'location':
            return (
            <div className={styles.name_div}>
                <input ref={first} type="text" placeholder="enter city name..."/>
                <input ref={last} type="text" placeholder="enter country name..."/>
            </div>
            )
    }
}


export default function ProfileSettingsPopup({setInfo, setPopup, text, setOverlay, type}) {
    const firstName = useRef(null);
    const lastName = useRef(null);
    const handleOnSave = () => {
        if (type != "about") {
            setInfo[0](firstName.current.value);
            setInfo[1](lastName.current.value);
        } else {
            setInfo(firstName.current.value);
        }
        setPopup(false);
        setOverlay(false);
    }

    return (
        <>
            <div className={styles.box}>
                <h3>Edit {text} </h3>
                {handleType(type, firstName, lastName)}
                <div className={styles.buttons}>
                    <button onClick={() => {
                        setPopup(false);
                        setOverlay(false);
                        }}>CANCEL</button>
                    <button onClick={handleOnSave}>SAVE</button>
                </div>
            </div>
        </>
    )
}