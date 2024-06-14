import styles from "./notificationList.module.css";
import RequestFollow from "../notificationItems/requestFollow.module";
import RegularItem from "../notificationItems/regularItem.module";
export default function NotificationList() {
    return (
        <>
            <div className={styles.list}>
                <div className={styles.delimeter}>
                    <p>Today</p>
                    <span></span>
                </div>
                <RequestFollow />
                <div className={styles.delimeter}>
                    <p>Earlier</p>
                    <span></span>
                </div>
                <RegularItem />
                <div className={styles.clear}>| Clear Notifications |</div>
            </div>
        </>
    );
}
