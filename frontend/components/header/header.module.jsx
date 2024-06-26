"use client";
import { useState, useEffect } from "react";
import Cookies from "js-cookie";
import { useRouter } from "next/navigation";
import styles from "./header.module.css";
import NotificationList from "../notificationList/notificationList.module";

export default function Header({overlay}) {
    const router = useRouter();
    const [tab, setTab] = useState(window.location.pathname);
    const [isLoggedIn, setIsLoggedIn] = useState(false);
    const [notificationList, setNotificationList] = useState(false);
    useEffect(() => {
        setTab(window.location.pathname);
        const token = Cookies.get("jwt");
        if (token) setIsLoggedIn(true);
    }, []);

    return(
        <>
            <div className={styles.parent}>
                <div className={styles.container}>
                    <div className={styles.header}>
                        <div onClick={() => {router.push("/")}} className={styles.logo}></div>
                        <div className={styles.header_buttons}>
                            <div onClick={() => {router.push("/")}} className={`${styles.button_wrapper} ${tab == "/" && styles.button_wrapper_active}`}>
                                <div className={`${styles.button} ${styles.home} ${tab == "/" && styles.active}`}></div>
                            </div>
                            <div onClick={() => {router.push(`/profile/${localStorage.getItem("userName")}`)}} className={`${styles.button_wrapper} ${tab.includes(`/profile/${localStorage.getItem("userName")}`) && styles.button_wrapper_active}`}>
                                <div className={`${styles.button} ${styles.user} ${tab.includes(`/profile/${localStorage.getItem("userName")}`) && styles.active}`}></div>
                            </div>
                            <div className={`${styles.button_wrapper} ${tab == "/explore" && styles.button_wrapper_active}`}>
                                <div className={`${styles.button} ${styles.explore} ${tab == "/explore" && styles.active}`}></div>
                            </div>
                            <div className={`${styles.button_wrapper} ${tab == "/friends" && styles.button_wrapper_active}`}>
                                <div className={`${styles.button} ${styles.friends} ${tab == "/friends" && styles.active}`}></div>
                                {isLoggedIn && <span className={styles.notify} style={{top: "-10px", right: "-10px"}}></span>}
                            </div>
                        </div>
                        {isLoggedIn &&
                        <>
                            <div className={styles.notification_wrapper}>
                                <div onClick={() => {setNotificationList(!notificationList)}} className={styles.notification}>
                                    {isLoggedIn && <span className={styles.notify}></span>}
                                </div>
                            </div>
                            {notificationList && <NotificationList />}
                        </>}
                        {!isLoggedIn &&
                        <div onClick={() => {router.push("/login")}} className={styles.login_button}>
                            <button>LOGIN</button>
                        </div>
                        }
                    </div>
                </div>
                {overlay && <div className={styles.overlay}></div>}
            </div>
        </>
    );
}
