import styled from '@emotion/styled';
import { Collapse, Modal, Spin } from 'antd';
import Checkbox from 'antd/es/checkbox';
import { memo, useCallback, useEffect, useMemo, useRef, useState } from 'react';
import { pdfjs } from 'react-pdf';
import { Document, Page } from 'react-pdf/dist/esm/entry.webpack5';

import { useGetMyUserProfile, usePutMyUserProfile } from '~/api';

import 'react-pdf/dist/esm/Page/AnnotationLayer.css';
import 'react-pdf/dist/esm/Page/TextLayer.css';

const { Panel } = Collapse;

pdfjs.GlobalWorkerOptions.workerSrc = 'pdf.worker.min.js';

const StyledModal = styled(Modal)`
  top: 10px;
`;

const StyledDocumentContainer = styled.div`
  overflow-y: auto;
  width: 100%;
`;

const StyledContainer = styled.div`
  display: flex;
  height: 100%;
  flex-direction: column;
`;

const StyledTitled = styled.div`
  padding-left: 10px;
`;

const StyledRequired = styled.div`
  color: red;
  position: absolute;
  z-index: 99;
  margin-left: -10px;
`;

const StyledTitledAll = styled.div`
  padding-left: 50px;
`;

const PdfRender = memo(({ url, osScrollEnd }: { url: string; osScrollEnd?: () => void }) => {
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
          osScrollEnd?.();
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

export type RenderDocuments = {
  title: string;
  required: boolean;
  name: string;
  url: string;
};

const AgreementConfirmModal: React.FC<{
  open: boolean;
  hideModal: () => void;
  docs: RenderDocuments[];
}> = ({ open, hideModal, docs }) => {
  const { data: my } = useGetMyUserProfile();
  const [confirmed, setConfirmed] = useState(false);
  const [confirmedDocs, setConfirmedDocs] = useState<string[]>([]);
  const mutation = usePutMyUserProfile();

  const ifConfirmedExist = useCallback(
    (docName: string) => {
      return confirmedDocs.includes(docName);
    },
    [confirmedDocs],
  );

  const onOk = useCallback(() => {
    mutation.mutate({
      userName: String(my?.userName),
      acceptedConsentToMailings: ifConfirmedExist('acceptedConsentToMailings'),
      acceptedPivacyPolicy: ifConfirmedExist('acceptedPivacyPolicy'),
      acceptedConsentToPersonalDataProcessing: ifConfirmedExist('acceptedConsentToPersonalDataProcessing'),
      acceptedOfferFoUser: ifConfirmedExist('acceptedOfferFoUser'),
      acceptedTermsOfUse: ifConfirmedExist('acceptedTermsOfUse'),
    });
  }, [ifConfirmedExist, mutation, my?.userName]);

  useEffect(() => {
    if (mutation.isSuccess) {
      hideModal();
    }
  }, [hideModal, mutation.isSuccess]);

  useEffect(() => {
    let newConfirmedState = true;

    docs.forEach((doc) => {
      if (doc.required && !confirmedDocs.includes(doc.name)) {
        newConfirmedState = false;
      }
    });

    setConfirmed(newConfirmedState);
  }, [confirmedDocs, docs]);

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
      width="80vw"
      bodyStyle={{
        height: '75vh',
        overflow: 'auto',
      }}
    >
      <StyledContainer>
        <StyledTitledAll>
          <Checkbox
            onChange={(e) => {
              const newDocs: string[] = [];

              if (e.target.checked) {
                docs.forEach((doc) => {
                  newDocs.push(doc.name);
                });
              }

              setConfirmedDocs(newDocs);
            }}
          >
            Подтверждаю, что прочитал и согласен со всеми соглашения
          </Checkbox>
        </StyledTitledAll>

        <Collapse accordion bordered={false} ghost>
          {docs.map((doc) => {
            return (
              <Panel
                key={doc.name}
                header={
                  <StyledTitled>
                    {doc.required ? <StyledRequired>*</StyledRequired> : undefined}
                    <Checkbox
                      onChange={(e) => {
                        const newDocs = [...confirmedDocs];

                        if (e.target.checked) {
                          newDocs.push(doc.name);
                        } else {
                          newDocs.splice(newDocs.findIndex((cdoc) => cdoc === doc.name));
                        }

                        setConfirmedDocs(newDocs);
                      }}
                      checked={confirmedDocs.includes(doc.name)}
                    >
                      Подтверждаю, что прочитал и согласен с {doc.title}
                    </Checkbox>
                  </StyledTitled>
                }
              >
                <PdfRender url={doc.url} />
              </Panel>
            );
          })}
        </Collapse>
      </StyledContainer>
    </StyledModal>
  );
};

export { AgreementConfirmModal };
