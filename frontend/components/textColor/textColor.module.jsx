import styles from "./textColor.module.css";

export default function TextColor({setColor, color, type}) {
    return (
        <>
            <div className={styles.box}>
                <h3>Choose {type} Color:</h3>
                <div className={styles.body}>
                    <input type="color" value={color} onChange={(e) => {setColor(e.target.value)}} />
                </div>
            </div>
        </>
    )
}