import styles from "./requestFollow.module.css";

export default function RequestFollow() {
    return (
        <>
            <div className={styles.item}>
                <div className={styles.header}>
                    <h3>FOLLOW REQUEST</h3>
                    <p className={styles.time}>1:34 pm</p>
                </div>
                <div className={styles.body}>
                    <div className={styles.photo}></div>
                    <div className={styles.info}>
                        <h3>Ammar Yasser</h3>
                        <p>ARTIST</p>
                        <p>ammar.yasser.2732@gmail.com</p>
                    </div>
                </div>
                <div className={styles.options}>
                    <button className={styles.reject}>REJECT</button>
                    <button className={styles.accept}>ACCEPT</button>
                </div>
            </div>
        </>
    );
}
