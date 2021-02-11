import { serverBaseUrl } from "../server-urls"

export function buildImagePath(imagePath) {
    return serverBaseUrl + imagePath;
}