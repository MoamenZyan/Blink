"use client";
import styles from "./notificationList.module.css";
import FriendNotification from "../notificationItems/requestFollow.module";
import PostNotification from "../notificationItems/regularItem.module";
import { useEffect, useState } from "react";
import Image from "next/image";

export default function NotificationList({notifications, setTrigger, trigger}) {
    const [todayNotification, setTodayNotification] = useState([]);
    const [earlierNotification, setEarlierNotification] = useState([]);

    useEffect(() => {
        const now = new Date();
        const today = new Date(now.getFullYear(), now.getMonth(), now.getDate());

        setTodayNotification(notifications.filter((noti) => {
            const notiDate = new Date(noti.createdAt);
            return (
                notiDate.getFullYear() === today.getFullYear() &&
                notiDate.getMonth() === today.getMonth() &&
                notiDate.getDate() === today.getDate()
            );
        }));
        setEarlierNotification(notifications.filter((noti) => {
            const notiDate = new Date(noti.createdAt);
            return (
                notiDate.getFullYear() !== today.getFullYear() ||
                notiDate.getMonth() !== today.getMonth() ||
                notiDate.getDate() !== today.getDate()
            );
        }));
        console.log("iam here");
    }, [notifications]);

    return (
        <>
            <div className={styles.list}>
                {todayNotification.length > 0 && <div className={styles.delimeter}>
                    <p>Today</p>
                    <span></span>
                </div>}
                {todayNotification.map((noti) => (
                    <>{noti.type = "friend" ? <FriendNotification setTrigger={setTrigger} trigger={trigger} notification={noti} /> : <PostNotification />}</>
                ))}
                {earlierNotification.length > 0 && <div className={styles.delimeter}>
                    <p>Earlier</p>
                    <span></span>
                </div>}
                {earlierNotification.map((noti) => (
                    <>{noti.type = "friend" ? <FriendNotification notification={noti} /> : <PostNotification />}</>
                ))}
                {notifications.length == 0 ?
                    <div className={styles.no_notifications_div}>
                        <Image className={styles.photo} src={"/assets/photo1.svg"} width={50} height={50} alt=""/>
                        <p>There is no notifications yet!</p>
                    </div> :
                    <div className={styles.clear}>| Clear Notifications |</div>
                }
            </div>
        </>
    );
}
