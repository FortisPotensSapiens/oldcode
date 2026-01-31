import { disableBodyScroll, enableBodyScroll } from 'body-scroll-lock';
import { useEffect } from 'react';

type UseBodyOverflow = (active?: boolean) => void;

const useBodyOverflow: UseBodyOverflow = (active) =>
  useEffect(() => {
    if (active) {
      disableBodyScroll(window.document.body);
    } else {
      enableBodyScroll(window.document.body);
    }
  }, [active]);

export { useBodyOverflow };
