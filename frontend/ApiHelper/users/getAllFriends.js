// Gets all friends users
const baseURL = "http://localhost:8080/api/v1";
export default async function GetAllFriendsUsers() {
    return await fetch(`${baseURL}/friends`, {
        method: "GET",
        credentials: "include",
    })
    .then(res => res.json())
    .then(data => {
        return data
    });
}
