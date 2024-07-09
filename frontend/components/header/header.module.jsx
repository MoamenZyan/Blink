"use client";
import { useState, useEffect } from "react";
import Cookies from "js-cookie";
import { useRouter } from "next/navigation";
import styles from "./header.module.css";
import Image from "next/image";
import NotificationList from "../notificationList/notificationList.module";
import GetAllNotifications from "@/ApiHelper/notifications/getAllNotificationsAPI";

export default function Header({overlay}) {
    const router = useRouter();
    const [tab, setTab] = useState(window.location.pathname);
    const [friendRequestNotifi, setFriendRequestNotifi] = useState(false);
    const [postNotificationsNotifi, setPostNotificationsNotifi] = useState(false);
    const [friendPageNotifi, setFriendPageNotifi] = useState(false);
    const [friendPageNotifiLength, setFriendPageNotifiLength] = useState(false);
    const [friendRequestNotifiLength, setFriendRequestNotifiLength] = useState(0);
    const [postNotificationsLength, setpostNotificationsLength] = useState(0);
    const [notificationList, setNotificationList] = useState(false);
    const [notifications, setNotifications] = useState([]);
    const [trigger, setTrigger] = useState(false);


    useEffect(() => {
        setTab(window.location.pathname);
        const getInfo = async () => {
            const notifications = await GetAllNotifications();
            console.log(notifications);
            if (notifications) {
                const postNotifications = notifications.notifications.postNotifications?.$values.map((noti) => ({...noti, type: "friend"}));
                const friendNotifications = notifications.notifications.friendRequestNotifications?.$values.map((noti) => ({...noti, type: "friend"}));
                setFriendRequestNotifiLength((friendNotifications?.length || 0));
                setpostNotificationsLength((postNotifications?.length || 0));
                setNotifications(handleSortingNotifications(friendNotifications, postNotifications));
                if (localStorage.getItem("friendRequestNotificationsLength") && localStorage.getItem("postNotificationsLength")) {
                    if (Number(localStorage.getItem("friendRequestNotificationsLength")) < (friendNotifications?.length || 0)) {
                        setFriendRequestNotifi(true);
                    } else {
                        setFriendRequestNotifi(false);
                    }

                    if (localStorage.getItem("friendPageNotifiLength") < (friendNotifications?.length || 0)) {
                        setFriendPageNotifi(true);
                    } else {
                        setFriendPageNotifi(false);
                    }
                    
                    if (Number(localStorage.getItem("postNotificationsLength")) < (postNotifications?.length || 0)) {
                        setPostNotificationsNotifi(true);
                    } else {
                        setPostNotificationsNotifi(false);
                    }
                } else {

                    localStorage.setItem("friendPageNotifiLength", (friendNotifications?.length || 0));
                    localStorage.setItem("friendRequestNotificationsLength", (friendNotifications?.length || 0));
                    localStorage.setItem("postNotificationsLength", (postNotifications?.length || 0));
                }
            }
        }
        getInfo();
        const interval = setInterval(() => {
            getInfo();
        }, 10000);

        return () => clearInterval(interval);
    }, [trigger]);

    const handleSortingNotifications = (friendNotifications, postNotifications) => {
        const allNotifications = friendNotifications.concat(postNotifications);
        allNotifications.sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt));
        return allNotifications.filter((noti) => noti != undefined);
    }

    return(
        <>
            <div className={styles.parent}>
                <div className={styles.container}>
                    <div className={styles.header}>
                        <div onClick={() => {router.push("/")}} className={styles.logo}>
                            <Image className={styles.icon} src={"/assets/logo.svg"} width={10} height={10} alt=""/>
                        </div>
                        <div className={styles.header_buttons}>
                            <div onClick={() => {router.push("/")}} className={`${styles.button_wrapper} ${tab == "/" && styles.button_wrapper_active}`}>
                                <div className={`${styles.button} ${styles.home} ${tab == "/" && styles.active}`}>
                                <Image className={styles.icon} src={tab == "/" ? "/assets/home_active.svg" : "/assets/home.svg"} width={10} height={10} alt=""/>
                                </div>
                            </div>
                            <div onClick={() => {router.push(`/profile/${localStorage.getItem("userName")}`)}} className={`${styles.button_wrapper} ${tab.includes(`/profile/${localStorage.getItem("userName")}`) && styles.button_wrapper_active}`}>
                                <div className={`${styles.button} ${styles.user} ${tab.includes(`/profile/${localStorage.getItem("userName")}`) && styles.active}`}>
                                    <Image className={styles.icon} src={tab == `/profile/${localStorage.getItem("userName")}` ? "/assets/user_active.svg" : "/assets/user.svg"} width={10} height={10} alt=""/>
                                </div>
                            </div>
                            <div onClick={() => {
                                router.push("/friends");
                                localStorage.setItem("friendPageNotifiLength", friendRequestNotifiLength);
                                }} className={`${styles.button_wrapper} ${tab == "/friends" && styles.button_wrapper_active}`}>
                                <div className={`${styles.button} ${styles.friends} ${tab == "/friends" && styles.active}`}>
                                    <Image className={styles.icon} src={tab == "/friends" ? "/assets/friends_active.svg" : "/assets/friends.svg"} width={10} height={10} alt=""/>
                                    {friendPageNotifi && <span className={styles.notify} style={{top: "-5px", right: "-5px"}}></span>}
                                </div>
                            </div>
                        </div>
                        {
                        <>
                            <div className={styles.notification_wrapper}>
                                <div onClick={() => {
                                    setNotificationList(!notificationList);
                                    localStorage.setItem("friendRequestNotificationsLength", friendRequestNotifiLength);
                                    localStorage.setItem("postNotificationsLength", postNotificationsLength);
                                    setFriendRequestNotifi(false);
                                    setPostNotificationsNotifi(false);
                                    }} className={styles.notification}>
                                    <Image className={styles.icon} src={"/assets/notification.svg"} width={10} height={10} alt=""/>
                                    {postNotificationsNotifi || friendRequestNotifi && <span className={styles.notify}></span>}
                                </div>
                            </div>
                            {notificationList && <NotificationList setTrigger={setTrigger} trigger={trigger} notifications={notifications} />}
                        </>}
                    </div>
                </div>
                {overlay && <div className={styles.overlay}></div>}
            </div>
        </>
    );
}
