import styles from "./settingsList.module.css";

export default function SettingsList() {
    return(
        <>
            <div className={styles.settings}>
                <div className={styles.add}>
                    <div className={styles.icon_wrapper}>
                        <div className={styles.add_icon}></div>
                    </div>
                    <p>Add to your story</p>
                </div>
                <div className={styles.save}>
                    <div className={styles.icon_wrapper}>
                        <div className={styles.save_icon}></div>
                    </div>
                    <p>Saved posts</p>
                </div>
                <div className={styles.archive}>
                    <div className={styles.icon_wrapper}>
                        <div className={styles.archive_icon}></div>
                    </div>
                    <p>Archive</p>
                </div>
                <div className={styles.options}>
                    <div className={styles.icon_wrapper}>
                        <div className={styles.options_icon}></div>
                    </div>
                    <p>Options & settings</p>
                </div>
            </div>
        </>
    )
}
