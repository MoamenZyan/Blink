// Update info of User
const baseURL = "http://localhost:8080/api/v1";
export default async function UpdateUserInfo(form) {
    const formData = new URLSearchParams(form);
    return await fetch(`${baseURL}/users`, {
        method: "PATCH",
        credentials: "include",
        headers: {
            "Content-Type": "application/x-www-form-urlencoded",
        },
        body: formData
    }).then(res => {
        if (res.ok) {
            return true;
        } else {
            return false;
        }
    })
}