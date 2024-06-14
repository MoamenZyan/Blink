
import styles from "./ownStory.module.css";

export default function OwnStory() {

    return(
        <>
            <div className={styles.story_div}>
                <div className={styles.bodyWrapper}>
                    <div className={styles.body}>

                    </div>
                    <div className={styles.user_default}>

                    </div>
                    <div className={styles.views}>
                        <div className={styles.eye_icon}></div>
                        <p>360k Views</p>
                    </div>
                </div>
            </div>
        </>
    );
}
