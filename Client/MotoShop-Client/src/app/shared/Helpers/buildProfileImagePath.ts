import { serverBaseUrl } from "../server-urls"

export function buildProfileImagePath(imagePath) {
    return serverBaseUrl + imagePath;
}