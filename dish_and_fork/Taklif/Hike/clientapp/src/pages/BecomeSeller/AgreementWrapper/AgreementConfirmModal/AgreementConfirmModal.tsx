import styled from '@emotion/styled';
import { Checkbox, Modal, Spin } from 'antd';
import { CheckboxChangeEvent } from 'antd/es/checkbox';
import { memo, useCallback, useEffect, useMemo, useRef, useState } from 'react';
import { pdfjs } from 'react-pdf';
import { Document, Page } from 'react-pdf/dist/esm/entry.webpack5';

import { usePutAcceptTermOfService } from '~/api/v1/usePutAcceptTermOfService';

import { IS_STORE_AGREEMENT_STORAGE_KEY } from '../types';

import 'react-pdf/dist/esm/Page/AnnotationLayer.css';
import 'react-pdf/dist/esm/Page/TextLayer.css';

pdfjs.GlobalWorkerOptions.workerSrc = 'pdf.worker.min.js';

const StyledModal = styled(Modal)`
  top: 10px;
`;

const StyledCheckboxContaienr = styled.div`
  margin-top: 0.5rem;
`;

const StyledDocumentContainer = styled.div`
  overflow-y: auto;
  width: 100%;
`;

const StyledContainer = styled.div`
  display: flex;
  height: 100%;
  flex-direction: column;
  justify-content: space-between;
`;

const PdfRender = memo(({ url, osScrollEnd }: { url: string; osScrollEnd: () => void }) => {
  const containerRef = useRef<null | HTMLDivElement>(null);
  const [numPages, setNumPages] = useState<number | null>(0);
  const pages = useMemo(() => {
    return [...Array(numPages).keys()];
  }, [numPages]);

  const onLoadSuccess = useCallback(
    ({ numPages: pages }) => {
      if (pages !== numPages) {
        setNumPages(pages);
      }
    },
    [numPages],
  );

  useEffect(() => {
    const el = containerRef.current;

    if (el) {
      const onScrolled = () => {
        if (el.offsetHeight + el.scrollTop + 200 >= el.scrollHeight) {
          osScrollEnd();
        }
      };

      el.addEventListener('scroll', onScrolled);

      return () => {
        el.removeEventListener('scroll', onScrolled);
      };
    }

    // eslint-disable-next-line react/display-name
    return () => {
      return null;
    };
  }, [containerRef, osScrollEnd]);

  return (
    <StyledDocumentContainer ref={containerRef}>
      <Document
        file={{ url }}
        onLoadSuccess={onLoadSuccess}
        loading={
          <StyledContainer>
            <Spin />
          </StyledContainer>
        }
        renderMode="canvas"
      >
        {pages
          .map((x, i) => i + 1)
          .map((page) => {
            return (
              <Page
                pageNumber={page}
                loading={
                  <StyledContainer>
                    <Spin />
                  </StyledContainer>
                }
                key={page}
                width={parseInt(String(containerRef.current?.clientWidth), 10) - 50}
              />
            );
          })}
      </Document>
    </StyledDocumentContainer>
  );
});

PdfRender.displayName = 'PdfRender';

const AgreementConfirmModal: React.FC<{
  open: boolean;
  hideModal: () => void;
  url: string;
}> = ({ open, hideModal, url }) => {
  const [viewed, setViwed] = useState(false);
  const [confirmed, setConfirmed] = useState(false);
  const mutation = usePutAcceptTermOfService();

  const onAgreed = useCallback((e: CheckboxChangeEvent) => {
    setConfirmed(e.target.value);
  }, []);

  const osScrollEnd = useCallback(() => {
    setViwed(true);
  }, []);

  const onOk = useCallback(() => {
    localStorage.setItem(IS_STORE_AGREEMENT_STORAGE_KEY, 'true');
    hideModal();
  }, [hideModal]);

  return (
    <StyledModal
      closable={false}
      title="Добро пожаловать"
      cancelButtonProps={{
        style: {
          display: 'none',
        },
      }}
      confirmLoading={mutation.isLoading}
      okButtonProps={{
        disabled: !confirmed,
      }}
      okType="primary"
      open={open}
      onOk={onOk}
      okText="Продолжить"
      width="80vh"
      bodyStyle={{
        height: '75vh',
      }}
    >
      <StyledContainer>
        <PdfRender url={url} osScrollEnd={osScrollEnd} />
        <StyledCheckboxContaienr>
          <Checkbox disabled={!viewed} onChange={onAgreed} value={viewed}>
            Принять лицензионное соглашение
          </Checkbox>
        </StyledCheckboxContaienr>
      </StyledContainer>
    </StyledModal>
  );
};

export { AgreementConfirmModal };
