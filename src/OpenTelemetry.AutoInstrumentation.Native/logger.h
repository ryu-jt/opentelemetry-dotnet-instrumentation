#pragma once

#include <Windows.h>
#include <functional>
#include <sstream>
#include <string>

#include "string.h"
#include "logger_impl.h"

namespace trace
{

class Logger
{
public:
    using OnTextHandler = std::function<void(const std::wstring&)>;

private:
    Logger()                    = delete;
    Logger(Logger&)             = delete;
    Logger(Logger&&)            = delete;
    Logger& operator=(Logger&)  = delete;
    Logger& operator=(Logger&&) = delete;

    static OnTextHandler onTextHandler;

    static void TriggerOnTextEvent(const std::wstring& text)
    {
        if (onTextHandler)
        {
            onTextHandler(text);
        }
    }

public:
    template <typename... Args>
    static void Debug(const Args&... args)
    {
         //std::ostringstream stream;
         //(stream << ... << ToString(args));
         //std::wstring text = StringToWString(stream.str());
         //TriggerOnTextEvent(L"[Debug] " + text);
    }

    template <typename... Args>
    static void Info(const Args&... args)
    {
         std::ostringstream stream;
         (stream << ... << ToString(args));
         std::wstring text = StringToWString(stream.str());
         TriggerOnTextEvent(L"[Info] " + text);
    }

    template <typename... Args>
    static void Warn(const Args&... args)
    {
        std::ostringstream stream;
        (stream << ... << ToString(args));
        std::wstring text = StringToWString(stream.str());
        TriggerOnTextEvent(L"[Warn] " + text);
    }

    template <typename... Args>
    static void Error(const Args&... args)
    {
        std::ostringstream stream;
        (stream << ... << ToString(args));
        std::wstring text = StringToWString(stream.str());
        TriggerOnTextEvent(L"[Error] " + text);
    }

    template <typename... Args>
    static void Critical(const Args&... args)
    {
        std::ostringstream stream;
        (stream << ... << ToString(args));
        std::wstring text = StringToWString(stream.str());
        TriggerOnTextEvent(L"[Critical] " + text);
    }

    static void SetOnTextHandler(const OnTextHandler& handler)
    {
        onTextHandler = handler;
    }

    static void EnableDebug(bool enable)
    {
        // Implementation...
    }

    static bool IsDebugEnabled()
    {
        // Implementation...
        return true;
    }

    static void Flush()
    {
        // Implementation...
    }

    static void Shutdown()
    {
        // Implementation...
    }

private:
    template <typename T>
    static std::string ToString(const T& arg)
    {
        std::ostringstream stream;
        stream << arg;
        return stream.str();
    }

    static std::string ToString(const std::wstring& wstr)
    {
        return WStringToString(wstr);
    }

    static std::string WStringToString(const std::wstring& wstr)
    {
        if (wstr.empty())
        {
            return std::string();
        }

        int         sizeNeeded = WideCharToMultiByte(CP_UTF8, 0, &wstr[0], (int)wstr.size(), NULL, 0, NULL, NULL);
        std::string strTo(sizeNeeded, 0);
        WideCharToMultiByte(CP_UTF8, 0, &wstr[0], (int)wstr.size(), &strTo[0], sizeNeeded, NULL, NULL);
        return strTo;
    }

    static std::wstring StringToWString(const std::string& str)
    {
        if (str.empty())
        {
            return std::wstring();
        }

        int          sizeNeeded = MultiByteToWideChar(CP_UTF8, 0, &str[0], (int)str.size(), NULL, 0);
        std::wstring wstrTo(sizeNeeded, 0);
        MultiByteToWideChar(CP_UTF8, 0, &str[0], (int)str.size(), &wstrTo[0], sizeNeeded);
        return wstrTo;
    }
};

} // namespace trace
