"use client"
import { useRouter } from "next/navigation";
import { useRef } from "react";
import styles from "./profilePhotoList.module.css";

export default function ProfilePhotoList({handleUpload, isMine, stories, id}) {
    const input = useRef(null);
    const router = useRouter();
    const handleOnClick = () => {
        input.current.click();
    }

    return (
        <>
            <div className={styles.list}>
                <div className={styles.button}>
                    <div className={styles.user_photo}></div>
                    <div>See profile photo</div>
                </div>
                {isMine && <>
                    <span className={styles.line}></span>
                    <div className={styles.button}>
                        <div className={styles.upload_photo}></div>
                        <div onClick={handleOnClick}>
                            <p>Choose profile photo</p>
                            <input onChange={() => {handleUpload(input)}} ref={input} type="file" name="" id="" />
                        </div>
                    </div>
                </>}
                {stories && <>
                    <span className={styles.line}></span>
                    <div onClick={() => {router.push(`/stories/${id}`)}} className={styles.button}>
                        <div className={styles.story}></div>
                        <p>{isMine == true ? "See your story" : "See user story"}</p>
                    </div>
                </>}
            </div>
        </>
    );
}
