// Gets all users
const baseURL = "http://localhost:8080/api/v1";
export default async function GetAllUsers() {
    return await fetch(`${baseURL}/users`, {
        method: "GET",
        credentials: "include",
    })
    .then(res => res.json())
    .then(data => {
        return data
    });
}
