"use client";
import styles from "./page.module.css";
import Input from "@/components/input/input.module";
import { useRouter } from "next/navigation";
import { useRef, useState } from "react";
import Login from "@/ApiHelper/Authentication/login";
import Spinner from "@/components/spinner/spinner.module";

export default function LoginPage() {
    const [loading, setLoading] = useState(false);
    const form = useRef(null);
    const router = useRouter();

    const submit = async (event) => {
        event.preventDefault();
        setLoading(true);
        if (await Login(new FormData(form.current))) {
            router.push("/");
            setLoading(false);
        } else {
            setLoading(false);
            console.log("no");
        }
    }

    return (
        <>
            <div className={styles.background}>
                <div className={styles.center}>
                    <div className={styles.left}>
                        <div className={styles.logo}></div>
                        <div className={styles.form_div}>
                            <h2>SIGN IN</h2>
                            <form ref={form}>
                                <Input type="text" name={"Username"} placeholder="Enter Your Username" password={false}/>
                                <Input type="password" name={"Password"} placeholder="Enter Your Password" password={true}/>
                                <p className={styles.forget}>Forgot password?</p>
                                <div className={styles.button}>
                                    {!loading && <> <button onClick={submit}>Sign in</button>
                                    <i className="fa-solid fa-arrow-right"></i></>}
                                    {loading && <Spinner />}
                                </div>
                                <p className={styles.signup}>Don't have an account? <span onClick={() => {router.push("/signup")}}>Sign Up!</span></p>
                            </form>
                        </div>
                        <div className={styles.slogan}>
                                <div className={styles.slogan_left}>
                                        Capture <br/> <span>your</span>
                                </div>
                                <div className={styles.slogan_right}>
                                        favorite <br/> <span>moments !</span>
                                </div>
                        </div>
                        <div className={styles.right}>
                            <div className={styles.topright}>
                                <span style={{height: "300px"}}></span>
                                <span style={{height: "350px"}}></span>
                                <span style={{height: "400px"}}></span>
                            </div>
                            <div className={styles.photo}></div>
                            <div className={styles.bottomright}>
                                <span style={{width: "300px"}}></span>
                                <span style={{width: "250px"}}></span>
                                <span style={{width: "200px"}}></span>
                                <span style={{width: "150px"}}></span>
                                <span style={{width: "100px"}}></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </>
    )
}