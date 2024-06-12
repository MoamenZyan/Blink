"use client";

import styles from "./input.module.css";
import { useState } from "react";

export default function Input(props) {
    const [passwordVisible, setPasswordVisible] = useState(true);
    const [type, setType] = useState(props.type);

    const eyeClick = () => {
        if (passwordVisible) {
            setType("text");
        } else {
            setType("password");
        }
        setPasswordVisible(!passwordVisible);
    }

    return (
        <>
            <div className={styles.input_div}>
                <input name={props.name} type={type} placeholder={props.placeholder} />
                {props.password && <i onClick={eyeClick} className={type == "password" ? "fa-solid fa-eye" : "fa-solid fa-eye-slash"}></i>}
            </div>
        </>
    )
}