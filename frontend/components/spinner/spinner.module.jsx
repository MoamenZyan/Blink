import styles from "./spinner.module.css";

export default function Spinner({width, height, border, color}) {
    return (
        <>
            <div className={styles.spinner} style={{width: `${width}px`, height: `${height}px`, borderWidth: `${border}px`, borderTopColor: color}}></div>
        </>
    )
}