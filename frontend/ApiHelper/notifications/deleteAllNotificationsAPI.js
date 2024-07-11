// Delete all notification
const baseURL = "http://localhost:8080/api/v1";
export default async function DeleteAllNotifications() {
    return await fetch(`${baseURL}/notifications`, {
        method: "Delete",
        credentials: "include"
    })
    .then(res => res.json())
    .then((data) => {return data});
}
