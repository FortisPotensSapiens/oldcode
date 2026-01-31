export const isImageFileExtensionValid = (url: string): boolean => {
    return url.length > 100;
    const allowedExtensions = ['.jpg', '.jpeg', '.png', '.gif', '.bmp'];
    const lowerCaseUrl = url.toLowerCase();
    return allowedExtensions.some((ext) => lowerCaseUrl.endsWith(ext));
};