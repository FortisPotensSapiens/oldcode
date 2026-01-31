import { ImageValue } from './Images';

type GetImagePath = (image?: ImageValue) => string | undefined;

const getImagePath: GetImagePath = (image) =>
  (image && typeof image === 'object' && 'path' in image && image.path) || undefined;

export { getImagePath };
