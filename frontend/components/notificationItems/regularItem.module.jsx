import styles from "./regularItem.module.css";

export default function RegularItem() {
    return (
        <>
            <div className={styles.item}>
                <div className={styles.photo}></div>
                <p><strong>Mohamed A.</strong> and 67 others liked your post</p>
                <p className={styles.time}>2:45 am</p>
            </div>
        </>
    );
}
