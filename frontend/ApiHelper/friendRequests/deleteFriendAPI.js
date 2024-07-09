
// Delete friend
const baseURL = "http://localhost:8080/api/v1";
export default async function DeleteFriend(id) {
    return await fetch(`${baseURL}/user/${id}/delete-friend`, {
        method: "DELETE",
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
