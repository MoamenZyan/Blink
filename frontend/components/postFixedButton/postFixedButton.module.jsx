"use client";
import styles from "./postFixedButton.module.css";
import Cookies from "js-cookie";
import { useEffect, useState } from "react";

export default function PostFixedButton({setCreatePost}) {
    const [isLogged, setIsLogged] = useState(false);
    useEffect(() => {
        const token = Cookies.get("jwt");
        if (token) setIsLogged(true);
    }, []);

    return (
        <>
            {isLogged && <div className={styles.button}>
                <div onClick={() => {setCreatePost(true)}} className={styles.star_icon}></div>
            </div>}
        </>
    )
}
