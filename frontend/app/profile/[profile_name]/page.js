import styles from "./page.module.css";
import Header from "@/components/header/header.module"
import SettingsList from "@/components/settings_list/settingsList.module";
import PostsWrapper from "@/components/postsWrapper/postsWrapper.module";
import UserInfoSection from "@/components/user_info_section/userInfoSection.module";

export default function ProfilePage(props) {
    return(
        <>
            <Header />
            <div className={styles.container}>
                <div className={styles.left}>
                    <SettingsList />
                </div>
                <div className={styles.center}>
                    <UserInfoSection />
                    <PostsWrapper />
                </div>
                <div className={styles.right}></div>
            </div>
        </>
    )
}
