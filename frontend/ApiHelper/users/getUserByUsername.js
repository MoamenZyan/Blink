
// Gets user information by his username
const baseURL = "http://localhost:8080/api/v1";
export default async function GetUserByUsername(username) {
    return await fetch(`${baseURL}/user/${username}`, {
        method: "GET",
        credentials: "include",
    })
    .then(res => res.json())
    .then(data => {
        return data
    });
}
