import { PartnerCreateModel } from '~/types';

const STORAGE_KEY = 'dnf:become-seller';

const getStored = (): Partial<PartnerCreateModel> => {
  const stored = window.localStorage.getItem(STORAGE_KEY);

  if (!stored) {
    return {};
  }

  try {
    const result = JSON.parse(stored);

    return result && typeof result === 'object' ? result : {};
  } catch (e) {
    return {};
  }
};

export { getStored, STORAGE_KEY };
