// check weather the current user has given reaction to a current post or not
const baseURL = "http://localhost:8080/api/v1";
export default async function HasReactionOnAPost(id) {
    return await fetch (`${baseURL}/post/has-reaction/${id}`, {
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