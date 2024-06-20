// Update About of User
const baseURL = "http://localhost:8080/api/v1";
export default async function UpdateUserAbout(form) {
    const formData = new URLSearchParams(form);
    return await fetch(`${baseURL}/user-about`, {
        method: "POST",
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