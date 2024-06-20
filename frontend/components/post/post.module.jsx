"use client";
import styles from "./post.module.css";
import { formatDistanceToNow } from 'date-fns';
import { useState, useEffect } from "react";
import { useRouter } from "next/navigation";
import AddReactionOnAPost from "@/ApiHelper/reactions/addReactionOnAPost";
import DeleteReactionFromAPost from "@/ApiHelper/reactions/deleteReactionFromAPost";
import HasReactionOnAPost from "@/ApiHelper/reactions/checkReactionOnAPost";
import PostOptions from "../postOptions/postOptions.module";
import Image from "next/image";

export default function Post({post}) {
    const router = useRouter();
    const [liked, setLiked] = useState(false);
    const [likeCount, setLikeCount] = useState(post.reactions.$values.length);
    const [commentsCount, setCommentsCount] = useState(post.comments.$values.length)
    const [optionsList, setOptionsList] = useState(false);

    const handleLike = async () => {
        if (liked) {
            setLiked(false);
            if (likeCount > 0)
                setLikeCount(count => count - 1);
            await DeleteReactionFromAPost(post.id);
        } else {
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

    const like = () => {
        if (liked) {
            if (likeCount > 0) {
                setLikeCount(likeCount - 1);
            }
            setLiked(false);
        } else {
            setLikeCount(likeCount + 1);
            setLiked(true);
        }
    }

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
                            <h2 onClick={() => {router.push(`/profile/${post.username}`)}}>@{post.username}</h2>
                            <div style={{display: "flex", alignItems: "center"}}>
                                <div className={styles.privacy_icon}>
                                    <Image src={iconPath} width={15} height={15} alt="privacy"/>
                                </div>
                                <span className={styles.dot}>·</span>
                                <p>{formatDistanceToNow(new Date(post.createdAt), {addSuffix: true})}</p>
                            </div>
                        </div>
                    </div>
                    <div className={styles.settings_wrapper}>
                        <div onClick={() => {setOptionsList(!optionsList)}} className={styles.settings}></div>
                    </div>
                </div>
                <div className={styles.body}>
                    <div className={styles.caption}>
                        {post.caption}
                    </div>
                    {post.photo != "null" && <img className={styles.body_photo} src={post.photo} />}
                </div>
                <div className={styles.reactions}>
                    <div className={styles.left}>
                        <div onClick={handleLike} className={styles.like}>
                            <div className={styles.icon_wrapper}>
                                <div className={styles.like_icon} style={{backgroundColor: liked && "#FCB341"}}></div>
                            </div>
                            <div className={styles.like_count}>{likeCount} Blinks</div>
                        </div>
                        <div className={styles.comment}>
                            <div className={styles.icon_wrapper}>
                                <div className={styles.comment_icon}></div>
                            </div>
                            <div className={styles.comment_count}>{commentsCount} Comments</div>
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
                {optionsList && <PostOptions post={post} setOptions={setOptionsList} />}
            </div>
        </>
    );
}