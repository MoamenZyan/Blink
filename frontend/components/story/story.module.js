import styles from "./story.module.css";

export default function Story() {
    return (
        <>
            <div className={styles.story_div}>
                <div className={styles.body}>
                    <p>this is asdas dfgfdg dfgasdfa  dfgdfg loremd sdfdsfsdf dasdsad</p>
                </div>
                <div className={styles.info}>
                    <div className={styles.photo}>
                    </div>
                    <div className={styles.user_info}>
                        <h3>default user</h3>
                        <p>1h ago</p>
                    </div>
                </div>
            </div>
        </>
    );
}
