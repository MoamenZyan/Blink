// give like to a post
const baseURL = "http://localhost:8080/api/v1";
export default async function AddReactionOnAPost(id) {
    return await fetch (`${baseURL}/reaction-post/${id}`, {
        method: "GET",
        credentials: "include",
    })
    .then((res) => {
        if (res.ok)
            return true;
        else
            return false;
    });
}