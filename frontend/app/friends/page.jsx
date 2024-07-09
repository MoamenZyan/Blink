"use client";
import { useEffect, useState } from "react";
import styles from "./page.module.css";
import Header from "@/components/header/header.module";
import GetAllUsers from "@/ApiHelper/users/getAllUsers";
import Image from "next/image";
import { useRouter } from "next/navigation";
import Friend from "@/components/friend/friend.module";
import GetAllNonFriendsUsers from "@/ApiHelper/users/getAllNonFriends";

export default function FriendsPage() {
    const router = useRouter();
    const [users, setUsers] = useState([]);
    const [searchedUsers, setSearchedUsers] = useState([]);
    const [searchType, setSearchType] = useState("username");
    const [searchValue, setSearchValue] = useState("");
    const [trigger, setTrigger] = useState(false);

    useEffect(() => {
        const getInfo = async () => {
            const users = await GetAllNonFriendsUsers();
            if (users) {
                setUsers(users.users.$values);
                setSearchedUsers(users.users.$values);
            }
        }
        getInfo();
    }, [trigger]);


    const handleFiltering = (value) => {
        if (value == "") {
            setSearchedUsers(users);
            return;
        }

        if (searchType == "username") {
            const filtered = users.filter((user) => user.username.toLowerCase().includes(value.toLowerCase()));
            setSearchedUsers(filtered);
        } else if (searchType == "country") {
            const filtered = users.filter((user) => user.country?.toLowerCase().includes(value.toLowerCase()));
            setSearchedUsers(filtered);
        } else if (searchType == "city") {
            const filtered = users.filter((user) => user.city?.toLowerCase().includes(value.toLowerCase()));
            setSearchedUsers(filtered);
        }
    }

    return (
        <>
            <Header overlay={false}/>
            <div className={styles.parent}>
                <div className={styles.container}>
                    <div className={styles.content}>
                        <div className={styles.search_div}>
                            <select onChange={(e) =>  {
                                setSearchType(type => e.target.value);
                                handleFiltering(searchValue)
                            }}>
                                <option value="username">Username</option>
                                <option value="country">Country</option>
                                <option value="city">City</option>
                            </select>
                            <input onChange={(e) => {
                                handleFiltering(e.target.value);
                                setSearchValue(e.target.value);
                            }} type="text" placeholder="search for a person..."/>
                            <Image src={"/assets/search.svg"} width={35} height={35}/>
                        </div>
                        <div className={styles.users}>
                            {searchedUsers.map((user) => (
                                <Friend user={user} setTrigger={setTrigger} trigger={trigger}/>
                            ))}
                        </div>
                    </div>
                </div>
            </div>
        </>
    )
}