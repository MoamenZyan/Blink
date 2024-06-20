import styles from "./storyUpload.module.css";
import Image from "next/image";

export default function StoryUpload({imageURL, backgroundColor, text, textColor}) {
    return (
        <>
            <div className={styles.box}>
                <div className={styles.left}>
                    <p>Uploading...</p>
                    <p>ST<div className={styles.animation}>
                        <Image src={"/assets/circle.svg"} width={22} height={22} alt=""/>
                        <Image className={styles.arrow} src={"/assets/arrow-up.svg"} width={20} height={20} alt=""/>
                        </div>RY</p>
                </div>
                <div className={styles.right}>
                    {imageURL == null ? <div className={styles.background} style={{backgroundColor: `${backgroundColor}`}}></div> :
                        <Image src={imageURL} width={100} height={100} alt="" style={{objectFit: "cover"}}/>
                    }
                    <p style={{color: `${textColor}`}}>{text}</p>
                </div>
                <div className={styles.bottom}></div>
            </div>
        </>
    )
}
