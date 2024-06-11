import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { checkMessage } from '@/services/shareService';
import { NavigateFunction, useNavigate } from 'react-router-dom';
import useSignalR from '@/services/useSignalR';
import CodeEditor from '@/components/code-editor';
import { Button } from '@/components/ui/button';
import { UserNav } from '@/components/user-nav';
import { useStateContext } from '@/contexts/ContextProvider'
import AdminDialog from '@/components/dialogs/admin-dialog';
import {
    ResizableHandle,
    ResizablePanel,
    ResizablePanelGroup,
} from "@/components/ui/resizable"
import { ScrollArea } from "@/components/ui/scroll-area"
import { useSnippetContext } from '@/contexts/SnippetContext';
import { Textarea } from "@/components/ui/textarea"
import { runCode } from '@/services/coderunService';
import { RunCodeResponse } from '@/abstract/coderunTypes';
import { Loader2 } from "lucide-react"
import CountdownTimer from '@/components/ui/countdown-timer';

const CodePage: React.FC = () => {
    const { sessionId } = useParams<{ sessionId: string }>();
    const navigate = useNavigate();
    const { codeValue, messageObject, sendMessageToGroup, sendSelectionToGroup } = useSignalR(sessionId || "");
    const [language] = useState('csharp');
    const { role } = useStateContext();
    const { mainSnippet, currentCode, expirationDate } = useSnippetContext();
    const [executedCode, setExecutedCode] = useState<string>("");
    const [isLoading, setIsLoading] = useState(false);

    const checkAndFetchSession = async (sessionId: string, navigate: NavigateFunction) => {
        try {
            const sessionExists = await checkMessage(sessionId);
            if (!sessionExists) {
                navigate('/404');
            }
        } catch (error) {
            console.error("Failed to fetch session:", error);
        }
    };

    const handleNavigate = () => {
        navigate('/');
    }

    const runCodeAndShowOutput = async () => {
        setIsLoading(true);
        try {
            const response = await runCode(currentCode || "", 1) as RunCodeResponse;
            setExecutedCode(response.output);
        } catch (error) {
            console.error('Error executing code:', error);
        } finally {
            setIsLoading(false);
        }
    };

    useEffect(() => {
        checkAndFetchSession(sessionId || "", navigate);
        if (sessionId)
            localStorage.setItem('sessionId', sessionId)
    }, [sessionId]);

    return (
        <>
            <Button onClick={handleNavigate} className='fixed bottom-0 left-0 m-4 z-10'>
                Назад
            </Button>
            <div className='h-screen flex flex-col overflow-hidden'>
                <div className="border-b h-12">
                    <div className="flex items-center px-4">
                        {expirationDate && (
                            <div className='flex gap-2 font-medium'>
                                <p>Сессия закончится через</p>
                                <CountdownTimer expirationDate={expirationDate} />
                            </div>
                        )}
                        <div className="ml-auto flex items-center mt-2 space-x-4">

                            {
                                role === "admin" ? (
                                    <AdminDialog />
                                ) : (
                                    <div></div>
                                )
                            }
                            <UserNav />
                        </div>
                    </div>
                </div>
                <div className='flex-grow overflow-auto'>
                    <ResizablePanelGroup
                        direction="horizontal"
                    >
                        <ResizablePanel defaultSize={80} minSize={50} className="relative">
                            <CodeEditor
                                codeValue={codeValue}
                                messageObject={messageObject!}
                                language={language}
                                sendMessageToGroup={sendMessageToGroup}
                                sendSelectionToGroup={sendSelectionToGroup}
                                sessionId={sessionId || ""}
                            />
                            {/* <div className='absolute bottom-0 right-12 m-4 z-10'>
                                <Select defaultValue='csharp' onValueChange={(value) => setLanguage(value)}>
                                    <SelectTrigger>
                                        <SelectValue placeholder="Язык" />
                                    </SelectTrigger>
                                    <SelectContent>
                                        <SelectItem value="javascript">JavaScript</SelectItem>
                                        <SelectItem value="typescript">TypeScript</SelectItem>
                                        <SelectItem value="csharp">C#</SelectItem>
                                        <SelectItem value="java">Java</SelectItem>
                                    </SelectContent>
                                </Select>
                            </div> */}
                        </ResizablePanel>
                        <ResizableHandle />
                        {mainSnippet ? (
                            <ResizablePanel defaultSize={20}>
                                <ResizablePanelGroup direction="vertical">
                                    <ResizablePanel defaultSize={75} minSize={10}>
                                        <ScrollArea className='h-[100%]'>
                                            <div className='p-4'>
                                                <h2 className='text-lg font-semibold'>Информация по задаче:</h2>
                                                {mainSnippet !== null ? (
                                                    <>
                                                        <p className='font-semibold'>Автор:</p>
                                                        <p>{mainSnippet.authorName}</p>
                                                        <p className='font-semibold'>Направление:</p>
                                                        <p>{mainSnippet.getDirectionLabel()}</p>
                                                        <p className='font-semibold'>Сложность:</p>
                                                        <p>{mainSnippet.getLevelLabel()}</p>
                                                        <p className='font-semibold'>Вопрос:</p>
                                                        <p>{mainSnippet.mainQuestion}</p>
                                                        <p className='font-semibold'>Ответ:</p>
                                                        <p>{mainSnippet.solution}</p>
                                                        <p className='font-semibold'>Дополнительные вопросы:</p>
                                                        {mainSnippet.additionalQuestions.map(quest => (
                                                            <p>{quest}</p>
                                                        ))} 
                                                    </>
                                                ) : (
                                                    <p>нет инфы</p>
                                                )}
                                            </div>
                                        </ScrollArea>
                                    </ResizablePanel>
                                    <ResizableHandle withHandle={true} />
                                    <ResizablePanel defaultSize={25} minSize={10}>
                                        <ScrollArea className='h-[100%]'>
                                            <div className='p-4'>
                                                <Button onClick={runCodeAndShowOutput} disabled={isLoading}>
                                                    {isLoading ? (
                                                        <>
                                                            <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                                                            Выполняется
                                                        </>
                                                    ) : (
                                                        'Выполнить'
                                                    )}
                                                </Button>
                                                <Textarea className='mt-4 min-h-[150px]' disabled placeholder='Результат кода' value={executedCode} />
                                            </div>
                                        </ScrollArea>
                                    </ResizablePanel>
                                </ResizablePanelGroup>
                            </ResizablePanel>
                        ) : (
                            <ResizablePanel defaultSize={20}>
                                <ResizablePanelGroup direction="vertical">
                                    <ResizablePanel defaultSize={25} minSize={10}>
                                        <ScrollArea className='h-[100%]'>
                                            <div className='p-4'>
                                                <Button onClick={runCodeAndShowOutput} disabled={isLoading}>
                                                    {isLoading ? (
                                                        <>
                                                            <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                                                            Выполняется
                                                        </>
                                                    ) : (
                                                        'Выполнить'
                                                    )}
                                                </Button>
                                                <Textarea className='mt-4 min-h-[150px]' disabled placeholder='Результат кода' value={executedCode} />
                                            </div>
                                        </ScrollArea>
                                    </ResizablePanel>
                                </ResizablePanelGroup>
                            </ResizablePanel>
                        )}

                    </ResizablePanelGroup>

                </div>
            </div>

        </>
    );
};

export default CodePage;