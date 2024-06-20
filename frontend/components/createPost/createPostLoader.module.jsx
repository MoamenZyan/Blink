
import styles from "./createPostLoader.module.css";
import Image from "next/image";

export default function CreatePostLoader({imageURL}) {
    return (
        <>
            <div className={styles.box}>
                <div className={styles.left}>
                    <p>Uploading...</p>
                    <p>P<div className={styles.animation}>
                        <Image src={"/assets/circle.svg"} width={22} height={22} alt=""/>
                        <Image className={styles.arrow} src={"/assets/arrow-up.svg"} width={20} height={20} alt=""/>
                        </div>ST</p>
                </div>
                <div className={styles.right}>
                    {imageURL == null ? <Image src={"/assets/default_upload.svg"} width={100} height={100} alt=""/> :
                        <Image src={imageURL} width={100} height={100} alt="" style={{objectFit: "cover", borderRadius: "25px"}}/>
                    }
                </div>
                <div className={styles.bottom}></div>
            </div>
        </>
    )
}
