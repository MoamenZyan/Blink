"use client"
import Cookies from "js-cookie";
import styles from "./userCard.module.css";
import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import Image from "next/image";
import Logout from "@/ApiHelper/Authentication/logout";

export default function UserCard({user}) {
    const router = useRouter();
    const [list, setList] = useState(false);
    const [isLogged, setIsLogged] = useState(false);

    const handleLogout = async () => {
        if (await Logout()) {
            localStorage.removeItem('userId');
            localStorage.removeItem('userName');
            Cookies.remove("jwt");
            router.push("/login")
        }
    }

    const listClick = () => {
        setList(!list);
    }

    useEffect(() => {
        const token = Cookies.get("jwt");
        if (token) setIsLogged(true);
    }, []);

    if (!user) {
        return <div>Loading...</div>;
    }

    return (
        <>
            <div className={`${styles.card}`}>
                {!isLogged && <div className={`${styles.parent_overlay}`}>
                    <div className={styles.text}>
                        <button onClick={() => {router.push("/login")}}>LOGIN</button>
                    </div>
                    <div className={`${styles.overlay}`}></div>
                </div>}
                <div onClick={() => {router.push(`/profile/${localStorage.getItem("userName")}`)}} className={styles.info}>
                    <div className={styles.user_photo}>
                        {user.photo == "null" ? <Image src={"/assets/user_default.svg"} width={80} height={80} alt="" /> :
                        <img className={styles.photo} src={user.photo} alt=""/>
                        }
                    </div>
                    <div className={styles.user_info}>
                        <div>
                            <h2>{user.firstName} {user.lastName}</h2>
                        </div>
                        <div className={styles.user_info_details}>
                            <div className={styles.followings}>
                                <h4>Following</h4>
                                <p>100</p>
                            </div>
                            <div className={styles.followers}>
                                <h4>Followers</h4>
                                <p>1.4M</p>
                            </div>
                            <div className={styles.posts}>
                                <h4>Posts</h4>
                                <p>{user.posts.$values.length}</p>
                            </div>
                        </div>
                    </div>
                </div>
                <span className={styles.line}></span>
                <div className={styles.more_info}>
                    <div className={styles.about}>
                        <p>{user.about}</p>
                    </div>
                    <div className={styles.location}>
                        <p>Egypt, Alexandria</p>
                    </div>
                    <div className={styles.email}>
                        <p>{user.email}</p>
                    </div>
                </div>
                <div className={`${styles.list} ${list ? styles.open : ''}`}>
                    <div onClick={() => {router.push("/profile/settings")}} className={styles.account}>
                        <div className={styles.account_icon}></div>
                        <p>Account Management</p>
                    </div>
                    <div className={styles.security}>
                        <div className={styles.security_icon}></div>
                        <p>Security</p>
                    </div>
                    <div className={styles.help}>
                        <div className={styles.help_icon}></div>
                        <p>Help & Support</p>
                    </div>
                    <div className={styles.feedback}>
                        <div className={styles.feedback_icon}></div>
                        <p>Give Feedback</p>
                    </div>
                    <div className={styles.logout} onClick={handleLogout}>
                        <div className={styles.logout_icon}></div>
                        <p>Logout</p>
                    </div>
                </div>
                <div onClick={listClick} className={styles.list_button}>
                    <div className={list ? styles.line_up : styles.line_down}></div>
                </div>
            </div>
        </>
    );
}
