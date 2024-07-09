"use client";
import styles from "./profilePhotoPost.module.css";
import { formatDistanceToNow } from 'date-fns';
import { useState, useEffect, useRef } from "react";
import { useRouter } from "next/navigation";
import AddReactionOnAPost from "@/ApiHelper/reactions/addReactionOnAPost";
import DeleteReactionFromAPost from "@/ApiHelper/reactions/deleteReactionFromAPost";
import HasReactionOnAPost from "@/ApiHelper/reactions/checkReactionOnAPost";
import PostOptions from "../postOptions/postOptions.module";
import Image from "next/image";
import PostCommentsSection from "../postComments/postComments.module";

export default function ProfilePhotoPost({post, setTrigger, trigger}) {
    const glint1 = useRef(null);
    const glint2 = useRef(null);
    const glint3 = useRef(null);
    const router = useRouter();
    const [liked, setLiked] = useState(false);
    const [likeCount, setLikeCount] = useState(post.reactions.$values.length);
    const [commentsCount, setCommentsCount] = useState(post.comments.$values.length);
    const [optionsList, setOptionsList] = useState(false);
    const [comments, setComments] = useState(false);

    const getIconPath = (privacy) => {
        switch (privacy) {
          case 'public':
            return '/assets/public.svg';
          case 'friends':
            return '/assets/friends.svg';
          case 'private':
            return '/assets/private.svg';
        }
    };

    const iconPath = getIconPath(post.privacy);

    const startLikeAnimation = () => {
        glint1.current.classList.add(styles.glint);
        glint2.current.classList.add(styles.glint);
        glint3.current.classList.add(styles.glint);
        setTimeout(() => {
            glint1.current.classList.remove(styles.glint);
            glint2.current.classList.remove(styles.glint);
            glint3.current.classList.remove(styles.glint);
        }, 2000);
    }

    const handleLike = async () => {
        if (liked) {
            setLiked(false);
            if (likeCount > 0)
                setLikeCount(count => count - 1);
            await DeleteReactionFromAPost(post.id);
        } else {
            startLikeAnimation();
            setLiked(true);
            setLikeCount(count => count + 1);
            await AddReactionOnAPost(post.id);
        }
    }

    useEffect(() => {
        const checkReaction = async () => {
            const result = await HasReactionOnAPost(post.id);
            if (result)
                setLiked(true);
        }
        checkReaction();
    }, []);

    return (
        <>
            <div className={styles.post_div}>
                <div className={styles.header}>
                    <div className={styles.info}>
                        <div className={styles.user_photo_wrapper}>
                            {post.userPhoto == "null" && <div className={styles.user_photo}></div>}
                            {post.userPhoto != "null" && <img className={styles.user_photo} src={post.userPhoto} alt=""/>}
                        </div>
                        <div className={styles.user_info}>
                            <h2 onClick={() => {router.push(`/profile/${post.username}`)}}>{post.username} <span className={styles.thin}>changed his profile photo</span></h2>
                            <div style={{display: "flex", alignItems: "center"}}>
                                <div className={styles.privacy_icon}>
                                    <Image src={iconPath} width={15} height={15} alt="privacy"/>
                                </div>
                                <span className={styles.dot}>·</span>
                                <p>{formatDistanceToNow(new Date(post.createdAt), {addSuffix: true})}</p>
                            </div>
                        </div>
                    </div>
                    <div onClick={() => {setOptionsList(!optionsList)}} className={styles.settings_wrapper}>
                        <div className={styles.settings}></div>
                    </div>
                </div>
                <div className={styles.body}>
                    <div className={styles.banner_body}></div>
                    <img src={post.photo} alt=""/>
                </div>
                <div className={styles.reactions}>
                    <div className={styles.left}>
                        <div onClick={handleLike} className={styles.like}>
                            <div className={styles.glints}>
                                <Image ref={glint1} className={`${styles.glint1}`} src={"/assets/glint1.svg"} width={50} height={50}/>
                                <Image ref={glint2} className={`${styles.glint2}`} src={"/assets/glint2.svg"} width={50} height={50}/>
                                <Image ref={glint3} className={`${styles.glint3}`} src={"/assets/glint3.svg"} width={50} height={50}/>
                            </div>
                            <div className={styles.icon_wrapper}>
                                <div className={styles.like_icon} style={{backgroundColor: liked && "#FCB341"}}></div>
                            </div>
                            <div className={styles.like_count}>{likeCount} Blinks</div>
                        </div>
                        <div onClick={() => {setComments(!comments)}} className={styles.comment}>
                            <div className={styles.icon_wrapper}>
                                <div className={styles.comment_icon}></div>
                            </div>
                            <div className={styles.comment_count}>{commentsCount + post.replies.$values.length} Comments</div>
                        </div>
                        <div className={styles.share}>
                            <div className={styles.icon_wrapper}>
                                <div className={styles.share_icon}></div>
                            </div>
                            <div className={styles.share_count}>300 Shares</div>
                        </div>
                    </div>
                    <div className={styles.right}>
                        <div className={styles.icon_wrapper}>
                            <div className={styles.save}></div>
                        </div>
                    </div>
                </div>
                {optionsList && <PostOptions post={post} setOptions={setOptionsList}/>}
                {comments && <PostCommentsSection postId={post.id} isComments={comments} comments={post.comments.$values} setCommentCount={setCommentsCount} setTrigger={setTrigger} trigger={trigger}/>}
            </div>
        </>
    );
}