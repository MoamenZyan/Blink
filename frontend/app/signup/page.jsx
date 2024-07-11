"use client";
import styles from "./page.module.css";
import Input from "@/components/input/input.module";
import { useRouter } from "next/navigation";
import Signup from "@/ApiHelper/Authentication/signup";
import { useRef, useState } from "react";
import Spinner from "@/components/spinner/spinner.module";
import Image from "next/image";

export default function SignupPage() {
    const [loading, setLoading] = useState(false);
    const form = useRef(null);
    const router = useRouter();

    const createUser = async (event) => {
        event.preventDefault();
        setLoading(true);
        if (await Signup(new FormData(form.current))) {
            setLoading(false);
            console.log("Created");
            } else {
            setLoading(false);
            console.log("No");
        }
    }

    return (
        <>
            <div className={styles.background}>
                <div className={styles.center}>
                    <div className={styles.left}>
                        <div className={styles.logo}></div>
                        <div className={styles.form_div}>
                            <h2>SIGN UP</h2>
                            <form ref={form}>
                                <Input type="text" placeholder={"Username"} password={false} name={"Username"} />
                                <div className={styles.names}>
                                    <Input type="text" placeholder={"First Name"} password={false} name={"FirstName"} />
                                    <Input type="text" placeholder={"Last Name"} password={false} name={"LastName"} />
                                </div>
                                <Input type="text" placeholder={"Email"} password={false} name={"Email"} />
                                <div className={styles.names}>
                                    <Input type="text" placeholder={"City"} password={false} name={"City"} />
                                    <Input type="text" placeholder={"Country"} password={false} name={"Country"} />
                                </div>
                                <Input type="password" placeholder={"Password"} password={true} name={"Password"} />
                                <Input type="password" placeholder={"Confirm Your Password"} password={true} />
                                <div className={styles.button}>
                                    {!loading && <button onClick={(e) => {createUser(e)}}>Let's Go!</button>}
                                    {loading && <Spinner />}
                                </div>
                                <p className={styles.already}>Already has an account? <span onClick={() => {router.push("/login")}}>Log in</span></p>
                            </form>
                        </div>
                        <div className={styles.slogan}>
                            <div className={styles.slogan_left}>
                                <span>Stay</span> <br/> Updated!
                            </div>
                            <div className={styles.slogan_right}>
                                With a <br/> Blink
                            </div>
                        </div>
                        <div className={styles.right}>
                            <div className={styles.inner_right_wrapper}>
                            <Image className={styles.photo} src={"/assets/photo2.svg"} width={50} height={50}/>
                                <div className={styles.stars}></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </>
    )
}
