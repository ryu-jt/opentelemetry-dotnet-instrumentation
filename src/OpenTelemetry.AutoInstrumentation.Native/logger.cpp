// logger.cpp
#include "logger.h"

namespace trace
{
// Logger 클래스 정적 멤버 초기화
Logger::OnTextHandler Logger::onTextHandler = nullptr;
} // namespace trace
