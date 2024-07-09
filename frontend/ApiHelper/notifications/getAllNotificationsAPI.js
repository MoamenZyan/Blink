// Get all notification
const baseURL = "http://localhost:8080/api/v1";
export default async function GetAllNotifications() {
    return await fetch(`${baseURL}/notifications`, {
        method: "GET",
        credentials: "include"
    })
    .then(res => res.json())
    .then((data) => {return data});
}
