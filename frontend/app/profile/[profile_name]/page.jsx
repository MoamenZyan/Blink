"use client";
import styles from "./page.module.css";
import Cookies from "js-cookie";
import Header from "@/components/header/header.module"
import SettingsList from "@/components/settings_list/settingsList.module";
import PostsWrapper from "@/components/postsWrapper/postsWrapper.module";
import UserInfoSection from "@/components/user_info_section/userInfoSection.module";
import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";1
import GetUserByUsername from "@/ApiHelper/users/getUserByUsername";

export default function ProfilePage(props) {
    const router = useRouter();
    const [trigger, setTrigger] = useState(false);
    const [isLogged, setIsLogged] = useState(false);
    const [loaded, setLoaded] = useState(false);
    const [user, setUser] = useState({});
    const [posts, setPosts] = useState([]);

    useEffect(() => {
        const getUserInfo = async () => {
            const result = await GetUserByUsername(props.params.profile_name);
            if (!isNaN(props.params.profile_name)) {
                router.push("/");
            }
            const token = Cookies.get("jwt");
            if (token)
                setIsLogged(true);
            if (result.status) {
                setUser(result.user);
                setPosts(result.user.posts.$values);
                console.log(result.user.posts.$values);
                setLoaded(true);
            } else {
                router.push("/");
            }
        }
        getUserInfo();
    }, [trigger]);

    if (loaded) {
        return(
            <>
                <div className={styles.parent}>
                    <Header />
                    <div className={styles.container}>
                        <div className={styles.left}>
                            <SettingsList />
                        </div>
                        <div className={styles.center}>
                            <UserInfoSection setTrigger={setTrigger} trigger={trigger} user={user} isLogged={isLogged} />
                            <PostsWrapper posts={posts} />
                        </div>
                        <div className={styles.right}></div>
                    </div>
                </div>
            </>
        )
    }
}
