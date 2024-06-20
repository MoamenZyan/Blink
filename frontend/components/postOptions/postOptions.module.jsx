"use client";
import styles from "./postOptions.module.css";
import { useState, useEffect } from "react";
import { useRouter } from "next/navigation";

export default function PostOptions({post, setOptions}) {
    const router = useRouter();
    const [isMine, setIsMine] = useState(false);

    useEffect(() => {
        if (localStorage.getItem("userName") == post.username) {
            setIsMine(true);
        }
    }, []);

    return (
        <>
            <div className={styles.list}>
                <h4>Post Options</h4>
                <span className={styles.line}></span>
                <p onClick={() => {router.push(`/profile/${post.username}`)}}>VIEW USER PROFILE</p>
                <p>COPY POST LINK</p>
                <p>REPORT POST</p>
                {isMine && <>
                    <p className={styles.edit}>EDIT POST</p>
                    <p className={styles.delete}>DELETE POST</p>
                </>}
                <p onClick={() => {setOptions(false)}}>CANCEL</p>
            </div>
        </>
    )
}