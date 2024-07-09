
// Send friend request
const baseURL = "http://localhost:8080/api/v1";
export default async function SendFriendRequest(id) {
    return await fetch(`${baseURL}/user/${id}/send-request`, {
        method: "GET",
        credentials: "include"
    })
    .then(res => {
        if (res.ok) {
            return true;
        } else {
            return false;
        }
    })
}
