import { MouseEventHandler, useState } from 'react';

type UseThumb = (initial?: number) => [number, MouseEventHandler<HTMLButtonElement>];

const useThumb: UseThumb = (initial = 0) => {
  const [state, setState] = useState(initial);

  const change: MouseEventHandler<HTMLButtonElement> = (event) => {
    const value = event.currentTarget.getAttribute('data-index');

    if (!value) {
      return;
    }

    try {
      setState(parseInt(value, 10));
    } catch (e) {
      console.error(`Can't select image with index "${value}"`);
    }
  };

  return [state, change];
};

export default useThumb;
