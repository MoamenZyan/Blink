"use client"
import styles from "./userInfoSection.module.css";
import Cookies from "js-cookie";
import { useEffect, useState } from "react";

export default function UserInfoSection(props) {
    const [isLogged, setIsLogged] = useState(false);
    useEffect(() => {
        const token = Cookies.get("jwt");
        if (token) setIsLogged(true);
    }, []);
    return (
        <>
            <div className={styles.info}>
                <div className={styles.banner_photo_div}></div>
                <div className={styles.head}>
                    <div className={styles.user_info}>
                        <div className={styles.user_photo_div}></div>
                        <div className={styles.user_info_details}>
                            <h1>Moamen Zyan</h1>
                            <div style={{display: "flex", alignItems: "center"}}>
                                <div className={styles.location}></div>
                                <p>Cairo, Egypt</p>
                            </div>
                            <div className={styles.friends}>
                                <p>300 friends</p>
                                <div className={styles.friends_photos}>
                                    <div className={styles.friend_photo}></div>
                                    <div className={styles.friend_photo}></div>
                                    <div className={styles.friend_photo}></div>
                                    <div className={styles.friend_photo}></div>
                                    <div className={styles.friend_photo}></div>
                                </div>
                            </div>
                        </div>
                    </div>
                    {isLogged && <div className={styles.user_settings}>
                        <button>Add Friend</button>
                        <button>Edit Profile Info</button>
                    </div>}
                </div>
                <div className={styles.line}></div>
                <div className={styles.about}>
                    <h4>About</h4>
                    <p>this is aasdasd fdghdfg dfgdfg dfgsdfsdaf fdsfsdf hgfhfgh sdfsdf sdfsdfsd hgghgfh sdfsdfs dhbgfhgfh sdfsdf hfgh fgh sdfsdfs sdfsdf fghfgh </p>
                </div>
            </div>
        </>
    )
}
