
// Accept friend request
const baseURL = "http://localhost:8080/api/v1";
export default async function AcceptFriendRequest(id) {
    return await fetch(`${baseURL}/user/${id}/accept-request`, {
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
