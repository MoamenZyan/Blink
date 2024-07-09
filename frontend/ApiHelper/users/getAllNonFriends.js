// Gets all non friends users
const baseURL = "http://localhost:8080/api/v1";
export default async function GetAllNonFriendsUsers() {
    return await fetch(`${baseURL}/non-friends`, {
        method: "GET",
        credentials: "include",
    })
    .then(res => res.json())
    .then(data => {
        return data
    });
}
